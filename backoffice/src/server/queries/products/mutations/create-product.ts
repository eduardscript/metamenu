import { fetchGraphQL } from '@/utils'

export async function createProductMutation(
  tenantCode: number,
  name: string,
  description: string,
  price: number,
  tagCodes: string[]
): Promise<string> {
  const mutation = `
    mutation CreateProduct($tenantCode: Int!, $name: String!, $description: String!, $price: Decimal!, $tagCodes: [String!]!) {
      createProduct(command: {
        tenantCode: $tenantCode,
        name: $name,
        description: $description,
        price: $price,
        tagCodes: $tagCodes,
      }) {
        code
      }
    }
  `

  const result = await fetchGraphQL<{ createProduct: { code: string } }>(
    mutation,
    {
      tenantCode,
      name,
      description,
      price,
      tagCodes,
    }
  )

  return result.createProduct.code
}
