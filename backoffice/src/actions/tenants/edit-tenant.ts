'use server'

import paths from '@/paths'
import { fetchGraphQL } from '@/utils'
import { revalidatePath } from 'next/cache'
import { redirect } from 'next/navigation'

interface EditTenantFormState {
  errors?: {
    name?: string[]
    _server?: string[]
  }
}

export default async function editTenant(
  formState: EditTenantFormState,
  formData: FormData
): Promise<EditTenantFormState> {
  try {
    const fields = {
      code: parseInt(formData.get('code') as string),
      name: formData.get('name') as string,
    }

    await fetchGraphQL(
      `
      mutation UpdateTenant($code: Int!, $name: String!) {
        updateTenant(code: $code, name: $name) {
          code
        }
      }
    `,
      fields
    )
  } catch (err: unknown) {
    if (err instanceof Error) {
      return { errors: { _server: [err.message] } }
    }

    return { errors: { _server: ['Failed to update tenant.'] } }
  }

  revalidatePath('/')

  redirect(paths.tenant.home())
}
