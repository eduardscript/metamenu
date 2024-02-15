import { fetchGraphQL } from "@/utils";

export async function createTenantMutation(name: string): Promise<number> {
  const query = `
    mutation CreateTenant($name: String!) {
        createTenant(command: {
            name: $name
        }) {
            code
        }
    }
`;

  const tenants = await fetchGraphQL<{ createTenant: { code: number } }>(
    query,
    {
      name,
    }
  );

  return tenants.createTenant.code;
}
