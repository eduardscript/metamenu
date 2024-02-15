import { TagCategory } from "@/server/models/tag-category";
import { fetchGraphQL } from "@/utils";

export async function getAllTagCategories(
  tenantCode: number
): Promise<TagCategory[]> {
  const query = `
    query GetAllTagCategories($tenantCode: Int!) {
       allTagCategories(query: {
          tenantCode: $tenantCode
       }) { 
          code
       }
    }
  `;

  const result = await fetchGraphQL<{ allTagCategories: TagCategory[] }>(
    query,
    {
      tenantCode,
    }
  );

  return result.allTagCategories;
}
