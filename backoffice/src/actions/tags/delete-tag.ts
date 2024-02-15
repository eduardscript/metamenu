"use server";

import { revalidatePath } from "next/cache";
import paths from "@/paths";
import { deleteTagMutation } from "@/server/queries/tags/mutations/delete-tag";
import { ApiError } from "@/server/errors/api-error";

interface DeleteTagFormState {
  errors: {
    tenantCode?: string[];
    tagCategoryCode?: string[];
    code?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function deleteTag(
  formState: DeleteTagFormState,
  formaData: FormData
) {
  const fields = {
    tenantCode: parseInt(formaData.get("tenantCode") as string),
    tagCategoryCode: formaData.get("tagCategoryCode") as string,
    code: formaData.get("code") as string,
  };

  try {
    await deleteTagMutation(
      fields.tenantCode,
      fields.tagCategoryCode,
      fields.code
    );

    revalidatePath(paths.tags.home(fields.tenantCode, fields.tagCategoryCode));

    return {
      success: true,
    };
  } catch (error) {
    if (error instanceof ApiError) {
      return {
        errors: {
          tenantCode: error.errors["TenantCode"],
          tagCategoryCode: error.errors["TagCategoryCode"],
          code: error.errors["Code"],
        },
        success: false,
      };
    }

    return {
      errors: {
        _server: ["An error occurred while creating the tag"],
      },
      success: false,
    };
  }
}
