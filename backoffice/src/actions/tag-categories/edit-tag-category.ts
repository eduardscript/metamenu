"use server";

import { ApiError } from "@/server/errors/api-error";
import { renameTagCategoryCodeMutation } from "@/server/queries/tag-categories/mutations/rename-tag-category";
import { revalidatePath } from "next/cache";

interface UpdateTagCategoryFormState {
  errors?: {
    tenantCode?: string[];
    newTagCategoryCode?: string[];
    oldTagCategoryCode?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function editTagCategory(
  formState: UpdateTagCategoryFormState,
  formData: FormData
): Promise<UpdateTagCategoryFormState> {
  const fields = {
    tenantCode: parseInt(formData.get("tenantCode") as string) as number,
    oldTagCategoryCode: formData.get("oldTagCategoryCode") as string,
    newTagCategoryCode: formData.get("newTagCategoryCode") as string,
  };

  try {
    await renameTagCategoryCodeMutation(
      fields.tenantCode,
      fields.oldTagCategoryCode,
      fields.newTagCategoryCode
    );

    revalidatePath("/tag-categories?tenantCode=" + fields.tenantCode);

    return {
      success: true,
    };
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors["TenantCode"],
          oldTagCategoryCode: error.errors["OldTagCategoryCode"],
          newTagCategoryCode: error.errors["NewTagCategoryCode"],
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
