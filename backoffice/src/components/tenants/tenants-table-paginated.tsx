import { Tenant } from '@/server/models/tenant'
import React from 'react'
import DeleteDialog from './delete-dialog'
import Link from 'next/link'
import paths from '@/paths'

interface TenantTableProps {
  tenants: Tenant[]
}

export default function TenantsTable({ tenants }: TenantTableProps) {
  return (
    <table className="table">
      <thead>
        <tr>
          <th>Code</th>
          <th>Name</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {tenants.map(({ code, name, isEnabled }) => (
          <tr className="hover" key={code}>
            <td className="font-bold">{code}</td>
            <td>
              {name}
              <br />
              <span
                className={`mt-1 badge text-white text-xs ${
                  isEnabled ? 'badge-success' : 'badge-error'
                }`}
              >
                {isEnabled ? 'Enabled' : 'Disabled'}
              </span>
            </td>
            <td className="flex gap-2">
              <Link href={paths.tenant.edit(code)}>
                <button className="btn btn-sm bg-slate-400 text-white text-sm">
                  Edit
                </button>
              </Link>
              <DeleteDialog
                title={`Are you sure you want to delete tenant ${code}?`}
              >
                <input type="hidden" name="code" value={code} />
                <p className="text-sm italic">(This action cannot be undone)</p>
              </DeleteDialog>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
