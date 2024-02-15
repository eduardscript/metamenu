"use server";

import prisma from "@/database";
import paths from "@/paths";
import { revalidatePath } from "next/cache";

import { z } from "zod";

const updateTagSchema = z.object({
  id: z.string().transform((val) => parseInt(val)),
  tagCategoryId: z.string().transform((val) => parseInt(val)),
  name: z.string(),
});

interface EditTagFormState {
  errors: {
    id?: string[];
    tagCategoryId?: string[];
    name?: string[];
  };
  success: boolean;
}

export default async function editTag(
  formState: EditTagFormState,
  formaData: FormData
) {
  const fields = updateTagSchema.safeParse(Object.fromEntries(formaData));

  if (!fields.success) {
    console.error(fields.error);

    return {
      errors: fields.error.flatten(),
      success: false,
    };
  }

  // const updatedTag = await prisma.tag.update({
  //   where: {
  //     id: fields.data.id,
  //   },
  //   data: {
  //     tagCategoryId: fields.data.tagCategoryId,
  //     name: fields.data.name,
  //   },
  // });

  // revalidatePath(
  //   paths.tags.home(updatedTag.tenantId, updatedTag.tagCategoryId)
  // );

  return {
    success: true,
  };
}
