"use server";

import { ApiError } from "@/server/errors/api-error";
import { deleteTagCategoryMutation } from "@/server/queries/tag-categories/mutations/delete-tag-category";
import { revalidatePath } from "next/cache";

interface UpdateTagCategoryFormState {
  errors?: {
    tenantCode?: string[];
    tagCategoryCode?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function deleteTagCategory(
  formState: UpdateTagCategoryFormState,
  formData: FormData
): Promise<UpdateTagCategoryFormState> {
  const fields = {
    tenantCode: parseInt(formData.get("tenantCode") as string) as number,
    tagCategoryCode: formData.get("tagCategoryCode") as string,
  };

  try {
    await deleteTagCategoryMutation(fields.tenantCode, fields.tagCategoryCode);

    revalidatePath("/tag-categories?tenantCode=" + fields.tenantCode);

    return {
      success: true,
    };
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors["TenantCode"],
          tagCategoryCode: error.errors["TagCategoryCode"],
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
