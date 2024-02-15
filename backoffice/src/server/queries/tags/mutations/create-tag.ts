import { fetchGraphQL } from "@/utils";

export async function createTagMutation(
  tenantCode: number,
  tagCategoryCode: string,
  code: string
): Promise<string> {
  const query = `
    mutation CreateTag($tenantCode: Int!, $tagCategoryCode: String!, $code: String!) {
        createTag(command: {
            tenantCode: $tenantCode,
            tagCategoryCode: $tagCategoryCode,
            code: $code
        }) {
            code
        }
    }
`;

  const tag = await fetchGraphQL<{ createTag: { code: string } }>(query, {
    tenantCode,
    tagCategoryCode,
    code,
  });

  return tag.createTag.code;
}
