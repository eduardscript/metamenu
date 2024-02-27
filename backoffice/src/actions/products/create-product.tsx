'use server'

import { ApiError } from '@/server/errors/api-error'
import { createProductMutation } from '@/server/queries/products/queries/get-all-products'
import { revalidatePath } from 'next/cache'
import { redirect } from 'next/navigation'

export interface CreateProductFormState {
  errors?: {
    tenantCode?: string[]
    name?: string[]
    description?: string[]
    price?: string[]
    tagCodes?: string[]
    _server?: string[]
  }
}

export default async function createProduct(
  formState: CreateProductFormState,
  formData: FormData
): Promise<CreateProductFormState> {
  const fields = {
    tenantCode: parseInt(formData.get('tenantCode') as string) as number,
    name: formData.get('name') as string,
    description: formData.get('description') as string,
    price: parseFloat(formData.get('price') as string),
    tagCodes: JSON.parse(formData.get('tagCodes') as string),
  }

  console.log(fields.tagCodes)

  try {
    await createProductMutation(fields.tenantCode, fields)

    revalidatePath(`/products/?tenantCode=${fields.tenantCode}`)
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors['TenantCode'],
          name: error.errors['Name'],
          description: error.errors['Description'],
          price: error.errors['Price'],
          tagCodes: error.errors['TagCodes'],
        },
      }
    }

    return {
      errors: {
        _server: ['An error occurred while creating the product'],
      },
    }
  }

  redirect(`/products/?tenantCode=${fields.tenantCode}`)
}
