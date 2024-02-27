import CreateProductForm from '@/components/products/create-product-form'
import { getAllTagsWithTagCategories } from '@/server/queries/tags/queries/get-tags-with-tag-categories'

interface CreateProductPageParams {
  params: {
    tenantCode: string
  }
}

export default async function CreateProductPage({
  params,
}: CreateProductPageParams) {
  const tagCategoriesWithTags = await getAllTagsWithTagCategories(
    parseInt(params.tenantCode)
  )

  return (
    <div className="flex justify-center w-full flex-col items-center">
      <div className="w-full max-w-xl">
        <CreateProductForm
          tenantCode={parseInt(params.tenantCode)}
          tagCategoriesWithTags={tagCategoriesWithTags}
        />
      </div>
    </div>
  )
}
