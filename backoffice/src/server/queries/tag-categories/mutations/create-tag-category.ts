import { fetchGraphQL } from "@/utils";

export async function createTagCategoryMutation(
  tenantCode: number,
  code: string
): Promise<string> {
  const query = `
    mutation CreateTagCategory($tenantCode: Int!, $code: String!) {
        createTagCategory(command: {
            tenantCode: $tenantCode,
            code: $code
        }) {
            code
        }
    }
  `;

  const result = await fetchGraphQL<{ createTagCategory: { code: string } }>(
    query,
    {
      tenantCode,
      code,
    }
  );

  return result.createTagCategory.code;
}
