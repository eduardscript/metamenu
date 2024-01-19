import axios from 'axios';
import { GET_TENANTS } from './queries';

const api = axios.create({
    baseURL: 'http://localhost:5195/graphql',
    headers: {
        'Content-Type': 'application/json',
    },
});

export const getTenants = async () => {
    try {
        const response = await api.post(
            '',
            {
                query: GET_TENANTS,
            },
        );

        return response.data.data.allTenants;
    } catch (error) {
        console.error('Error fetching tenants:', error);
        throw error;
    }
};

export const createTenant = async (name: string) => {
    try {
        const response = await api.post(
            '',
            {
                query: `
            mutation CreateTenant($name: String!) {
                createTenant(command: { name: $name }) {
                    code
                }
            }
            `,
                variables: {
                    name,
                },
            },
        );
        return response.data;
    } catch (error) {
        console.error('Error creating tenant:', error);
        throw error;
    }
};

export const deleteTenant = async (code: number) => {
    try {
        const response = await api.post(
            '',
            {
                query: `
                mutation($code: Int!) {
                    deleteTenant(command: {
                        code: $code
                    }) {
                        isDeleted
                    }
                }
            `,
                variables: {
                    code,
                },
            },
        );
        return response.data;
    } catch (error) {
        console.error('Error deleting tenant:', error);
        throw error;
    }
};

export const toggleTenantStatus = async (code: number, isEnabled: boolean) => {
    try {
        const response = await api.post(
            '',
            {
                query: `
                mutation($code: Int!, $isEnabled: Boolean!) {
                    toggleTenantStatus(command: {
                        code: $code,
                        isEnabled: $isEnabled
                    }) {
                        statusUpdated
                    }
                }
            `,
                variables: {
                    code,
                    isEnabled,
                },
            },
        );
        return response.data;
    } catch (error) {
        console.error('Error deleting tenant:', error);
        throw error;
    }
};

export const getTagCategories = async (tenantCode: number) => {
    const response = await api.post(
        '',
        {
            query: `
            query($tenantCode: Int!) {
                allTagCategories(
                    query: {
                        tenantCode: $tenantCode,
                    }
                ) {
                    code
                }
            }
        `,
            variables: {
                tenantCode,
            },
        },
    );

    return response.data.data;
}

export const createTagCategory = async (tenantCode: number, code: string) => {
    const response = await api.post(
        '',
        {
            query: `
            mutation($tenantCode: Int!, $code: String!) {
                createTagCategory(
                    command: {
                        tenantCode: $tenantCode,
                        code: $code
                    }
                ) {
                    code
                }
            }
        `,
            variables: {
                tenantCode,
                code
            },
        },
    );

    return response.data.data;
}

export const getAllTagCategories = async (tenantCode: number) => {
    const response = await api.post(
        '',
        {
            query: `
            query($tenantCode: Int!) {
                allTagCategories(
                    query: {
                        tenantCode: $tenantCode
                    }
                ) {
                    code
                }
            }
        `,
            variables: {
                tenantCode
            },
        },
    );

    console.log(response.data.data.allTagCategories);
    
    return response.data.data.allTagCategories;
}

export const getAllTagsByTagCategoryCode = async (tenantCode: number, tagCategoryCode: string) => {
    const response = await api.post(
        '',
        {
            query: `
            query($tenantCode: Int!, $tagCategoryCode: String!) {
                allTagsByTagCategoryCode(
                    query: {
                        tenantCode: $tenantCode,
                        tagCategoryCode: $tagCategoryCode
                    }
                ) {
                    code
                }
            }
        `,
            variables: {
                tenantCode,
                tagCategoryCode
            },
        },
    );

    return response.data.data.allTagsByTagCategoryCode;
}

export const createTag = async (tenantCode: number, tagCategoryCode: string, code: string) => {
    const response = await api.post(
        '',
        {
            query: `
            mutation($tenantCode: Int!, $tagCategoryCode: String!,$code: String!) {
                createTag(
                    command: {
                        tenantCode: $tenantCode,
                        tagCategoryCode: $tagCategoryCode,
                        code: $code
                    }
                ) {
                    code
                }
            }
        `,
            variables: {
                tenantCode,
                tagCategoryCode,
                code
            },
        },
    );

    return response.data.data;
}

export const deleteTag = async (tenantCode: number, tagCategoryCode: string, code: string) => {
    const response = await api.post(
        '',
        {
            query: `
            mutation($tenantCode: Int!, $tagCategoryCode: String!,$code: String!) {
                deleteTag(
                    command: {
                        tenantCode: $tenantCode,
                        tagCategoryCode: $tagCategoryCode,
                        code: $code
                    }
                ) {
                    isDeleted
                }
            }
        `,
            variables: {
                tenantCode,
                tagCategoryCode,
                code
            },
        },
    );

    return response.data.data;
}