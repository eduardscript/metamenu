import { fetchGraphQL } from "@/utils";

export async function deleteTenantMutation(code: number): Promise<boolean> {
  const query = `
    mutation DeleteTenant($code: Int!) {
        deleteTenant(command: {
            code: $code
        }) {
            isDeleted
        }
    }
`;

  const tenants = await fetchGraphQL<{ deleteTenant: { isDeleted: boolean } }>(
    query,
    {
      code,
    }
  );

  return tenants.deleteTenant.isDeleted;
}
