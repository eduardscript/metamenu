'use client'

import editTenant from '@/actions/tenants/edit-tenant'
import { useRouter } from 'next/navigation'
import { useFormState } from 'react-dom'

interface EditTenantPageProps {
  tenantCode: string
}

export default function EditTenantPage({ tenantCode }: EditTenantPageProps) {
  const router = useRouter()
  const [state, action] = useFormState(editTenant, {
    errors: {},
  })

  const tenant = {
    code: 1,
    name: 'Tenant 1',
    isActive: true,
  }

  return (
    <div className="flex flex-col items-center gap-2">
      <div className="w-1/2">
        <h1 className="font-bold text-xl mb-4">Update Tenant</h1>
        <form action={action} className="flex flex-col">
          <input type="hidden" name="code" value={tenantCode} />
          <label htmlFor="name">Name</label>
          <input
            name="name"
            className="input input-bordered mb-1"
            placeholder="tenant name"
            defaultValue={tenant.name}
          />
          {state.errors?.name && (
            <span className="text-red-500 block">{state.errors.name}</span>
          )}

          <label htmlFor="isActive">Is Active</label>
          <input
            name="isActive"
            type="checkbox"
            className="toggle toggle-success"
            defaultChecked={tenant.isActive}
          />

          <div className="flex justify-end gap-2">
            <button
              className="btn flex-shrink-0"
              type="reset"
              onClick={() => router.back()}
            >
              Cancel
            </button>

            <button className="btn btn-warning flex-shrink-0" type="submit">
              Update
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
