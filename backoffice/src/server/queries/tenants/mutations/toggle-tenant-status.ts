import { fetchGraphQL } from "@/utils";

export async function toggleTenantStatusMutation(
  code: string,
  isEnabled: boolean
): Promise<boolean> {
  const query = `
    mutation ToggleTenantStatus($code: String!, $isEnabled: Boolean!) {
        toggleTenantStatus(command: {
            code: $code,
            isEnabled: $isEnabled
        }) {
            statusUpdated
        }
    }
`;

  const tenants = await fetchGraphQL<{ statusUpdated: boolean }>(query, {
    code,
    isEnabled,
  });

  return tenants.statusUpdated;
}
