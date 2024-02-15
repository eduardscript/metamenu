"use server";

import prisma from "@/database";
import { revalidatePath } from "next/cache";
import { z } from "zod";

const deleteTagCategorySchema = z.object({
  id: z.string().transform((val) => parseInt(val)),
});

interface DeleteTagCategoryFormState {
  errors: {
    id?: string[];
  };
  success: boolean;
}

export default async function deleteTagCategory(
  formState: DeleteTagCategoryFormState,
  formaData: FormData
) {
  const fields = deleteTagCategorySchema.safeParse(
    Object.fromEntries(formaData)
  );

  if (!fields.success) {
    return {
      errors: fields.error.flatten(),
      success: false,
    };
  }

  await prisma.tagCategory.delete({
    where: {
      id: fields.data.id,
    },
  });

  revalidatePath("/");

  return {
    success: true,
  };
}
