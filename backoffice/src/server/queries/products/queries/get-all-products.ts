import { fetchGraphQL } from '@/utils'

export async function getAllProductsQuery(tenantCode: number) {
  const query = `
      query GetAllProducts($tenantCode: Int!) {
        allProducts(query: { productFilter: { tenantCode: $tenantCode } }) {
          name
          description
          price
          tagCodes
        }
      }
    `

  const response = await fetchGraphQL<{
    allProducts: {
      name: string
      description: string
      price: number
      tagCodes: string[]
    }[]
  }>(query, { tenantCode })

  return response.allProducts
}

export async function createProductMutation(
  tenantCode: number,
  product: {
    name: string
    description: string
    price: number
    tagCodes: string[]
  }
) {
  const mutation = `
      mutation CreateProduct(
        $tenantCode: Int!
        $name: String!
        $description: String
        $price: Decimal!
        $tagCodes: [String!]!
      ) {
        createProduct(
          command: {
            tenantCode: $tenantCode
            name: $name
            description: $description
            price: $price
            tagCodes: $tagCodes
          }
        ) {
          code
        }
      }
    `

  return await fetchGraphQL(mutation, {
    tenantCode,
    ...product,
  })
}
