"use server";

import prisma from "@/database";
import paths from "@/paths";
import { ApiError } from "@/server/errors/api-error";
import { createTagMutation } from "@/server/queries/tags/mutations/create-tag";
import { revalidatePath } from "next/cache";

interface CreateTagFormState {
  errors: {
    tenantCode?: string[];
    tagCategoryCode?: string[];
    code?: string[];
    _server?: string[];
  };
  success: boolean;
}

export default async function createTag(
  formState: CreateTagFormState,
  formaData: FormData
) {
  const fields = {
    tenantCode: parseInt(formaData.get("tenantCode") as string),
    tagCategoryCode: formaData.get("tagCategoryCode") as string,
    code: formaData.get("code") as string,
  };

  try {
    await createTagMutation(
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
