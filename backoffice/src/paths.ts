const paths = {
  buildQueryString: (params: any) => {
    const query = Object.keys(params)
      .map((key) => key + "=" + params[key])
      .join("&");

    return `?${query}`;
  },
  tags: {
    path: "/tags",
    home(tenantId: number, tagCategoryCode: string) {
      return `/${paths.tags.path}${paths.buildQueryString({
        tenantId,
        tagCategoryCode,
      })}`;
    },
  },
};

export default paths;
