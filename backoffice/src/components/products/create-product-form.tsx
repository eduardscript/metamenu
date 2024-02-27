'use client'

import { useFormState } from 'react-dom'
import TagSelector from './tag-selector'
import createProduct from '@/actions/products/create-product'
import { TagsWithTagCategories } from '@/server/queries/tags/queries/get-tags-with-tag-categories'

export default function CreateProductForm({
  tenantCode,
  tagCategoriesWithTags,
}: {
  tenantCode: number
  tagCategoriesWithTags: TagsWithTagCategories[]
}) {
  const [state, action] = useFormState(createProduct, {
    errors: {},
  })

  return (
    <form action={action}>
      <input type="hidden" name="tenantCode" value={tenantCode} />
      <div className="flex flex-col justify-start my-4">
        <h1 className="font-bold text-xl">Add new product</h1>
        Create a new product to add to your menu.
      </div>
      <label htmlFor="name" className="block font-semibold">
        Product Name
      </label>
      <input
        type="text"
        name="name"
        className="input input-bordered w-full mb-2"
        placeholder="Enter a product name"
      />
      {state.errors?.name && (
        <p className="text-red-500 text-sm">{state.errors?.name}</p>
      )}
      <label htmlFor="description" className="block font-semibold">
        Description (optional)
      </label>
      <textarea
        name="description"
        className="w-full textarea textarea-bordered mb-2"
        placeholder="Add details about this product"
      />
      {state.errors?.description && (
        <p className="text-red-500 text-sm">{state.errors?.description}</p>
      )}
      <label htmlFor="price" className="block font-semibold">
        Price in euros
      </label>

      <input
        type="number"
        name="price"
        className="input input-bordered w-full mb-2"
        placeholder="0.00"
        step="0.01"
      />
      {state.errors?.price && (
        <p className="text-red-500 text-sm">{state.errors?.price}</p>
      )}
      <label htmlFor="tags" className="block font-semibold mb-2">
        Tags
      </label>
      <TagSelector tagCategoriesWithTags={tagCategoriesWithTags} />
      {state.errors?.tagCodes && (
        <p className="text-red-500 text-sm">{state.errors?.tagCodes}</p>
      )}
      {state.errors?._server && (
        <p className="text-red-500 text-sm">{state.errors?._server}</p>
      )}

      <button className="btn btn-primary" type="submit">
        Add Product
      </button>
    </form>
  )
}
