"use server";

import paths from "@/paths";
import { ApiError } from "@/server/errors/api-error";
import { updateTagMutation } from "@/server/queries/tags/mutations/update-tag";
import { revalidatePath } from "next/cache";

interface UpdateTagFormState {
  errors?: {
    tenantCode?: string[];
    code?: string[];
    newTagCode?: string[];
    newTagCategoryCode?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function editTagCategory(
  formState: UpdateTagFormState,
  formData: FormData
): Promise<UpdateTagFormState> {
  const fields = {
    tenantCode: parseInt(formData.get("tenantCode") as string) as number,
    code: formData.get("code") as string,
    newTagCode: formData.get("newTagCode") as string,
    newTagCategoryCode: formData.get("newTagCategoryCode") as string,
  };

  try {
    await updateTagMutation(fields);

    revalidatePath(
      paths.tags.home(fields.tenantCode, fields.newTagCategoryCode)
    );

    return {
      success: true,
    };
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors["TenantCode"],
          code: error.errors["Code"],
          newTagCode: error.errors["NewTagCode"],
          newTagCategoryCode: error.errors["NewTagCategoryCode"],
        },
        success: false,
      };
    }

    return {
      errors: {
        _server: ["An error occurred while updating the tag"],
      },
      success: false,
    };
  }
}
