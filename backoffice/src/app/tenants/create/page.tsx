'use client'

import createTenant from '@/actions/tenants/create-tenant'
import { useRouter } from 'next/navigation'
import { useFormState } from 'react-dom'

export default function CreateTenantPage() {
  const router = useRouter()
  const [state, action] = useFormState(createTenant, {
    errors: {},
  })

  return (
    <div className="flex flex-col items-center gap-2">
      <div className="w-1/2">
        <h1 className="font-bold text-xl mb-4">Create Tenant</h1>
        <form action={action} className="flex flex-col">
          <label htmlFor="name">Name</label>
          <input
            name="name"
            className="input input-bordered mb-1"
            placeholder="tenant name"
          />
          {state.errors?.name && (
            <span className="text-red-500 block">{state.errors.name}</span>
          )}

          <div className="flex justify-end gap-2">
            <button
              className="btn flex-shrink-0"
              type="reset"
              onClick={() => router.back()}
            >
              Cancel
            </button>

            <button className="btn btn-success flex-shrink-0" type="submit">
              Create
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
