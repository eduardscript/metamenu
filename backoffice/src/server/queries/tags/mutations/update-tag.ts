import { fetchGraphQL } from "@/utils";

interface UpdateTagCommand {
  tenantCode: number;
  code: string;
  newTagCode?: string;
  newTagCategoryCode?: string;
}

export async function updateTagMutation(
  input: UpdateTagCommand
): Promise<boolean> {
  const query = `
    mutation UpdateTag($input: UpdateTagCommand!) {
        updateTag(command: $input) {
            isUpdated
        }
    }
  `;

  const result = await fetchGraphQL<{ updateTag: { isUpdated: boolean } }>(
    query,
    {
      input,
    }
  );

  return result.updateTag.isUpdated;
}
