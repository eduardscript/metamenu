'use server'

import { ApiError } from '@/server/errors/api-error'
import { deleteTenantMutation } from '@/server/queries/tenants'
import { revalidatePath } from 'next/cache'

interface DeleteTenantFormState {
  errors?: {
    name?: string[]
    _server?: string[]
  }
}

export default async function deleteTenant(
  formState: DeleteTenantFormState,
  formData: FormData
): Promise<DeleteTenantFormState> {
  const fields = {
    code: parseInt(formData.get('code') as string),
  }

  try {
    await deleteTenantMutation(fields.code)
  } catch (err: unknown) {
    if (err instanceof ApiError) {
      return { errors: { name: err.errors['Name'] } }
    }

    return {
      errors: { _server: ['Failed to delete tenant.'] },
    }
  }

  revalidatePath('/')

  return {}
}
