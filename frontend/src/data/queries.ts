export const GET_TENANTS = `
  query GetTenants {
    allTenants {
      code
      name
      isEnabled
      createdAt
    }
  }
`;
