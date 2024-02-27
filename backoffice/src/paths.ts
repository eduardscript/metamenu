const paths = {
  buildQueryString: (params: any) => {
    const query = Object.keys(params)
      .map((key) => key + '=' + params[key])
      .join('&')

    return `?${query}`
  },
  tags: {
    path: '/tags',
    home(tenantId: number, tagCategoryCode: string) {
      return `/${paths.tags.path}${paths.buildQueryString({
        tenantId,
        tagCategoryCode,
      })}`
    },
  },
  tenant: {
    path: '/tenants',
    home() {
      return `${paths.tenant.path}`
    },
    create() {
      return `${paths.tenant.path}/create`
    },
    edit(code: number) {
      return `${paths.tenant.path}/${code}/edit`
    },
  },
  product: {
    path: '/products',
    home() {
      return `${paths.product.path}`
    },
    create(tenantId: number | string) {
      return `${paths.product.path}/${tenantId}/create`
    },
    edit(code: number) {
      return `${paths.product.path}/${code}/edit`
    },
  },
}

export default paths
