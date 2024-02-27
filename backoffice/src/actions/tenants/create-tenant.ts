'use server'

import { revalidatePath } from 'next/cache'
import { createTenantMutation } from '@/server/queries/tenants'
import { ApiError } from '@/server/errors/api-error'
import { redirect } from 'next/navigation'

interface CreateTenantFormState {
  errors?: {
    name?: string[]
    _server?: string[]
  }
}

export default async function createTenant(
  formState: CreateTenantFormState,
  formData: FormData
): Promise<CreateTenantFormState> {
  const fields = {
    name: formData.get('name') as string,
  }

  try {
    await createTenantMutation(fields.name)

    revalidatePath('/')
  } catch (err: unknown) {
    if (err instanceof ApiError) {
      return { errors: { name: err.errors['Name'] } }
    }

    return {
      errors: { _server: ['Failed to create tenant.'] },
    }
  }

  redirect('/')
}
