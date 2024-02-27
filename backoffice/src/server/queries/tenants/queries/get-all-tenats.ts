import { Tenant } from '@/server/models/tenant'
import { fetchGraphQL } from '@/utils'

export async function getAllTenantsQuery(): Promise<Tenant[]> {
  const query = ` { 
    allTenants {
      code
      name
      isEnabled
      createdAt
      }
    }`

  const tenants = await fetchGraphQL<{ allTenants: Tenant[] }>(query)

  return tenants.allTenants
}
