import { fetchGraphQL } from "@/utils";

export async function renameTagCategoryCodeMutation(
  tenantCode: number,
  oldTagCategoryCode: string,
  newTagCategoryCode: string
): Promise<boolean> {
  const query = `
    mutation RenameTagCategoryCode($tenantCode: Int!, $oldTagCategoryCode: String!, $newTagCategoryCode: String!) {
        renameTagCategoryCode(command: {
            tenantCode: $tenantCode,
            oldTagCategoryCode: $oldTagCategoryCode,
            newTagCategoryCode: $newTagCategoryCode
        }) {
            isUpdated
        }
    }
  `;

  const result = await fetchGraphQL<{
    renameTagCategoryCode: { isUpdated: boolean };
  }>(query, {
    tenantCode,
    oldTagCategoryCode,
    newTagCategoryCode,
  });

  console.log(result);

  return result.renameTagCategoryCode.isUpdated;
}
