import { Tag } from "@/server/models/tag";
import { fetchGraphQL } from "@/utils";

export async function getAllTagsByTagCategory(
  tenantCode: number,
  tagCategoryCode: string
): Promise<Tag[]> {
  const query = `
    query GetAllTagsByTagCategory($tenantCode: Int!, $tagCategoryCode: String!) {
       allTagsByTagCategoryCode(query: {
          tenantCode: $tenantCode,
          tagCategoryCode: $tagCategoryCode
       }) { 
          code
       }
    }
  `;

  const result = await fetchGraphQL<{
    allTagsByTagCategoryCode: Tag[];
  }>(query, {
    tenantCode: tenantCode,
    tagCategoryCode: tagCategoryCode,
  });

  return result.allTagsByTagCategoryCode;
}
