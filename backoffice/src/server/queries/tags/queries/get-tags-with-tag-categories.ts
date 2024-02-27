import { Tag } from '@/server/models/tag'
import { fetchGraphQL } from '@/utils'

export interface TagsWithTagCategories {
  tagCategoryCode: string
  tagCodes: string[]
}

export async function getAllTagsWithTagCategories(
  tenantCode: number
): Promise<TagsWithTagCategories[]> {
  const query = `
    query GetAllTagsWithTagCategories($tenantCode: Int!) {
       allTags(query: {
          tenantCode: $tenantCode
       }) { 
          tagCategoryCode
          code
       }
    }
  `

  const result = await fetchGraphQL<{
    allTags: Tag[]
  }>(query, {
    tenantCode: tenantCode,
  })

  const convertedResponse: { [tagCategory: string]: TagsWithTagCategories } = {}

  result.allTags.forEach((tag: Tag) => {
    const { tagCategoryCode, code } = tag
    if (!convertedResponse[tagCategoryCode]) {
      convertedResponse[tagCategoryCode] = {
        tagCategoryCode: tagCategoryCode,
        tagCodes: [],
      }
    }
    convertedResponse[tagCategoryCode].tagCodes.push(code)
  })

  console.log('convertedResponse', Object.values(convertedResponse))

  return Object.values(convertedResponse)
}
