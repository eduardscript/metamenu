"use server";

import { ApiError } from "@/server/errors/api-error";
import { createTagCategoryMutation } from "@/server/queries/tag-categories/mutations/create-tag-category";
import { revalidatePath } from "next/cache";

interface CreateTagCategoryFormState {
  errors?: {
    tenantCode?: string[];
    code?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function createTagCategory(
  formState: CreateTagCategoryFormState,
  formData: FormData
): Promise<CreateTagCategoryFormState> {
  const fields = {
    tenantCode: parseInt(formData.get("tenantCode") as string) as number,
    code: formData.get("code") as string,
  };

  try {
    await createTagCategoryMutation(fields.tenantCode, fields.code);

    revalidatePath("/");

    return {
      success: true,
    };
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors["TenantCode"],
          code: error.errors["Code"],
        },
        success: false,
      };
    }

    return {
      errors: {
        _server: ["An error occurred while creating the tag category"],
      },
      success: false,
    };
  }
}
