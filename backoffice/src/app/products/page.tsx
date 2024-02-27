import SelectTenant from '@/components/tag-categories/select-tenant'
import paths from '@/paths'
import { getAllProductsQuery } from '@/server/queries/products/queries/get-all-products'
import { getAllTenantsQuery } from '@/server/queries/tenants'
import Image from 'next/image'
import Link from 'next/link'
import { MdClose, MdDelete, MdEdit } from 'react-icons/md'

interface SearchParams {
  searchParams?: {
    tenantCode?: string
  }
}

export default async function ProductsPage({ searchParams }: SearchParams) {
  const tenantCode = searchParams?.tenantCode
    ? parseInt(searchParams.tenantCode)
    : null

  const tenants = await getAllTenantsQuery()

  const products = tenantCode && (await getAllProductsQuery(tenantCode))

  function renderedProducts(): JSX.Element | JSX.Element[] {
    if (!products || products.length === 0) {
      return <div>This tenant doesn't have products yet.</div>
    }

    return products.map((product) => (
      <li className="card mt-2" key={product.name}>
        <div className="flex justify-between items-center">
          <div className="flex items-center gap-4">
            <div className="w-12 h-12 bg-base-200 rounded"></div>
            <div className="flex flex-col">
              <span className="font-semibold">{product.name}</span>
              <span className="text-gray-400">
                {product.tagCodes.length}{' '}
                {product.tagCodes.length === 1 ? 'tag' : 'tags'}
              </span>
            </div>
          </div>
          <div>$ {(product.price as number).toFixed(2)}</div>
        </div>
      </li>
    ))
  }

  return (
    <div className="flex flex-col md:px-10 gap-4">
      <div className="flex gap-2 justify-between">
        <h1 className="font-bold text-xl">Manage Products</h1>
        {tenantCode && (
          <Link
            href={paths.product.create(tenantCode)}
            className="bg-white rounded-xl text-sm px-4 py-2"
          >
            Add Product
          </Link>
        )}
      </div>
      <SelectTenant tenants={tenants} />

      <h2 className="font-bold">All Products</h2>

      <ul>{renderedProducts()}</ul>
    </div>
  )
}
