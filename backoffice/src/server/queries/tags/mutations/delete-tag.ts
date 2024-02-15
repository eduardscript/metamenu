import { fetchGraphQL } from "@/utils";

export async function deleteTagMutation(
  tenantCode: number,
  tagCategoryCode: string,
  code: string
): Promise<boolean> {
  const query = `
    mutation DeleteTag($code: String!, $tenantCode: Int!, $tagCategoryCode: String!) {
        deleteTag(command: {
            code: $code,
            tenantCode: $tenantCode,
            tagCategoryCode: $tagCategoryCode
        }) {
            isDeleted
        }
    }
  `;

  const result = await fetchGraphQL<{ deleteTag: { isDeleted: boolean } }>(
    query,
    {
      code,
      tenantCode,
      tagCategoryCode,
    }
  );

  return result.deleteTag.isDeleted;
}
