"use server";

import prisma from "@/database";
import { revalidatePath } from "next/cache";

import { z } from "zod";

const updateTagCategorySchema = z.object({
  id: z.string().transform((val) => parseInt(val)),
  name: z.string(),
  tenantId: z.string().transform((val) => parseInt(val)),
});

interface UpdateTagCategoryFormState {
  errors: {
    name?: string[];
    tenantId?: string[];
  };
  success: boolean;
}

export default async function editTagCategory(
  formState: UpdateTagCategoryFormState,
  formaData: FormData
) {
  const fields = updateTagCategorySchema.safeParse(
    Object.fromEntries(formaData)
  );

  if (!fields.success) {
    return {
      errors: fields.error.flatten(),
      success: false,
    };
  }

  await prisma.tagCategory.update({
    where: {
      id: fields.data.id,
    },
    data: {
      name: fields.data.name,
      tenantId: fields.data.tenantId,
    },
  });

  revalidatePath("/tag-categories?tenantId=" + fields.data.tenantId);

  return {
    success: true,
  };
}
