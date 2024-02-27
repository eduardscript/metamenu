'use client'

import { TagsWithTagCategories } from '@/server/queries/tags/queries/get-tags-with-tag-categories'
import { useState } from 'react'

export default function TagSelector({
  tagCategoriesWithTags,
}: {
  tagCategoriesWithTags: TagsWithTagCategories[]
}) {
  const [tags, setTags] = useState<{
    database: TagsWithTagCategories[]
    product: Map<string, string[]>
  }>({
    database: tagCategoriesWithTags,
    product: new Map<string, string[]>(),
  })

  function handleTagCode(tagCategoryCode: string, tagCode: string) {
    setTags((prev) => {
      const updatedProduct = new Map(prev.product)

      // If tagCategoryCode doesn't exist in product tags, add it with the new tagCode
      if (!updatedProduct.has(tagCategoryCode)) {
        updatedProduct.set(tagCategoryCode, [tagCode])
      } else {
        const categoryTags = updatedProduct.get(tagCategoryCode) ?? []

        // If tagCode is already present, remove it
        if (categoryTags.includes(tagCode)) {
          removeDbTagCodeFromProductTags(tagCategoryCode, tagCode)
        } else {
          // Otherwise, add it to the existing tag codes
          updatedProduct.set(tagCategoryCode, [...categoryTags, tagCode])
        }
      }

      return {
        ...prev,
        product: updatedProduct,
      }
    })
  }

  function removeDbTagCodeFromProductTags(
    tagCategoryCode: string,
    tagCode: string
  ) {
    if (tags.product.has(tagCategoryCode)) {
      const updatedTags = tags.product
        .get(tagCategoryCode)
        ?.filter((tag) => tag !== tagCode)!

      setTags((prev) => {
        const updatedProduct = new Map(prev.product)

        updatedTags.length === 0
          ? updatedProduct.delete(tagCategoryCode)
          : updatedProduct.set(tagCategoryCode, updatedTags)

        return {
          ...prev,
          product: updatedProduct,
        }
      })
    }
  }

  const filteredTagCategories = tags.database.map(
    ({ tagCategoryCode, tagCodes }) => {
      const productTags = tags.product.get(tagCategoryCode) || []
      const filteredTagCodes = tagCodes.filter(
        (tagCode) => !productTags.includes(tagCode)
      )

      return {
        tagCategoryCode,
        tagCodes: filteredTagCodes,
      }
    }
  )

  const renderedDatabaseTagsByTagCategory = (
    tagCategoryWithTags:
      | { tagCategoryCode: string; tagCodes: string[] }[]
      | Map<string, string[]>
  ) => {
    if (tagCategoryWithTags instanceof Map) {
      return Array.from(tagCategoryWithTags).map(
        ([tagCategoryCode, tagCodes]) => {
          return (
            <div key={tagCategoryCode}>
              <h3 className="bg-black text-white font-semibold text-sm p-2">
                {tagCategoryCode}
              </h3>
              <ul>
                {tagCodes.map((tagCode) => (
                  <li
                    key={tagCode}
                    onClick={() =>
                      removeDbTagCodeFromProductTags(tagCategoryCode, tagCode)
                    }
                    className="bg-slate-500 text-white hover:bg-slate-700 p-2 cursor-pointer text-sm"
                  >
                    {tagCode}
                  </li>
                ))}
              </ul>
            </div>
          )
        }
      )
    }

    if (tagCategoryWithTags instanceof Array) {
      return tagCategoryWithTags.map((tagCategory) => {
        return (
          <div key={tagCategory.tagCategoryCode}>
            <h3 className="bg-black text-white font-semibold text-sm p-2">
              {tagCategory.tagCategoryCode}
            </h3>
            <ul>
              {tagCategory.tagCodes.map((tagCode) => (
                <li
                  key={tagCode}
                  onClick={() =>
                    handleTagCode(tagCategory.tagCategoryCode, tagCode)
                  }
                  className="bg-slate-500 text-white hover:bg-slate-700 p-2 cursor-pointer text-sm"
                >
                  {tagCode}
                </li>
              ))}
            </ul>
          </div>
        )
      })
    }
  }

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    const searchTerm = e.target.value.toLowerCase()

    if (searchTerm.length === 0) {
      setTags((prev) => ({
        ...prev,
        database: tagCategoriesWithTags,
      }))

      return
    }

    const searchedTagCategories = filteredTagCategories.map((tagCategory) => {
      // 1st. filter by tag category code
      const isToFilterByTagCategoryCode =
        tagCategory.tagCategoryCode.toLowerCase().includes(searchTerm) &&
        tagCategory.tagCodes.length > 0

      if (isToFilterByTagCategoryCode) {
        return tagCategory
      }

      // 2nd. filter by tag codes
      const filteredTagCodes = tagCategory.tagCodes.filter((tagCode) =>
        tagCode.toLowerCase().includes(searchTerm)
      )

      if (filteredTagCodes.length > 0) {
        return {
          tagCategoryCode: tagCategory.tagCategoryCode,
          tagCodes: filteredTagCodes,
        }
      }

      return null!
    })

    setTags((prev) => ({
      ...prev,
      database: searchedTagCategories.filter(Boolean),
    }))
  }

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Backspace') {
      setTags((prev) => ({
        ...prev,
        database: tagCategoriesWithTags,
      }))
    }
  }

  return (
    <div className="flex justify-between">
      <input
        type="hidden"
        name="tagCodes"
        value={JSON.stringify(Array.from(tags.product.values()).flat())}
      />

      <div>
        {tags.product.size < 1 ? (
          <p className="italic">no tags yet 😙</p>
        ) : (
          renderedDatabaseTagsByTagCategory(tags.product)
        )}
      </div>

      <div className="rounded-xl">
        <input
          type="text"
          className="input input-bordered mb-2"
          placeholder="Search..."
          onChange={handleSearch}
          onKeyDown={handleKeyDown}
        />
        {renderedDatabaseTagsByTagCategory(filteredTagCategories)}
      </div>
    </div>
  )
}
