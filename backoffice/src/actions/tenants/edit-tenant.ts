"use server";

import prisma from "@/database";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";

import { z } from "zod";

const updateTenantSchema = z.object({
  id: z.string().transform((val) => parseInt(val)),
  name: z.string().min(3),
  isActive: z.optional(z.string()).transform((val) => !!val),
});

interface EditTenantFormState {
  errors?: {
    name?: string[];

    _form?: string[];
  };
  success?: boolean | null;
}

export default async function editTenant(
  formState: EditTenantFormState,
  formData: FormData
) {
  const fields = updateTenantSchema.safeParse(Object.fromEntries(formData));

  if (!fields.success) {
    console.error(fields.error);

    return {
      success: false,
      errors: fields.error.flatten().fieldErrors,
    };
  }

  try {
    // TODO:EC: Implement the ability to update tenants in backend

    revalidatePath("/");

    return { success: true };
  } catch (err: unknown) {
    if (err instanceof Error) {
      return { errors: { _form: [err.message] } };
    }

    return { errors: { _form: ["Failed to update tenant."] } };
  }
}
