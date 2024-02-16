import { fetchGraphQL } from "@/utils";

export async function deleteTagCategoryMutation(
  tenantCode: number,
  tagCategoryCode: string
): Promise<boolean> {
  const query = `
    mutation DeleteTagCategory($tenantCode: Int!, $tagCategoryCode: String!) {
        deleteTagCategory(command: {
            tenantCode: $tenantCode,
            tagCategoryCode: $tagCategoryCode
        }) {
            isDeleted
        }
    }
  `;

  const result = await fetchGraphQL<{
    deleteTagCategory: { isDeleted: boolean };
  }>(query, {
    tenantCode,
    tagCategoryCode,
  });

  console.log(result);

  return result.deleteTagCategory.isDeleted;
}
