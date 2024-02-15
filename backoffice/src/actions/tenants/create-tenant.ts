"use server";

import { revalidatePath } from "next/cache";
import { z } from "zod";
import { createTenantMutation } from "@/server/queries/tenants";
import { ApiError } from "@/server/errors/api-error";

const createTenantSchema = z.object({
  name: z.string().min(3),
});

interface CreateTenantFormState {
  errors?: {
    name?: string[];
    _server?: string[];
  };
  success?: boolean;
}

export default async function createTenant(
  formState: CreateTenantFormState,
  formData: FormData
): Promise<CreateTenantFormState> {
  const fields = {
    name: formData.get("name") as string,
  };

  try {
    await createTenantMutation(fields.name);

    revalidatePath("/");

    return { success: true };
  } catch (err: unknown) {
    if (err instanceof ApiError) {
      return { errors: { name: err.errors["Name"] }, success: false };
    }

    return {
      errors: { _server: ["Failed to create tenant."] },
      success: false,
    };
  }
}
