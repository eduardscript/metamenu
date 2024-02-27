import SearchInput from '@/components/search-input'
import TenantsTable from '@/components/tenants/tenants-table-paginated'
import paths from '@/paths'
import { getAllTenantsQuery } from '@/server/queries/tenants'
import Link from 'next/link'
import { MdAdd } from 'react-icons/md'

export default async function Page({
  searchParams,
}: {
  searchParams?: {
    tenant?: string
  }
}) {
  const tenantQuery = searchParams?.tenant || ''

  const tenants = await getAllTenantsQuery()

  return (
    <div className="flex flex-col md:px-10 gap-4">
      <div className="flex gap-2 justify-between">
        <h1 className="font-bold text-xl">All tenants</h1>
        <Link
          href={paths.tenant.create()}
          className="bg-white rounded-xl text-sm px-4 py-2"
        >
          Add Tenant
        </Link>
      </div>

      <input
        className="input input-bordered rounded-xl w-full bg-slate-900"
        placeholder="tenant"
      />

      <TenantsTable tenants={tenants} />
    </div>
  )
}
