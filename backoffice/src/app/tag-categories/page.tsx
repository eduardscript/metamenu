import createTagCategory from "@/actions/tag-categories/create-tag-category";
import deleteTagCategory from "@/actions/tag-categories/delete-tag-category";
import editTagCategory from "@/actions/tag-categories/edit-tag-category";
import Dialog from "@/components/common/dialog/dialog";
import EntityCard from "@/components/common/entity-card";
import CreateTagCategoryForm from "@/components/tag-categories/forms/create-tag-category-form";
import DeleteTagCategoryForm from "@/components/tag-categories/forms/delete-tag-category-form";
import EditTagCategoryForm from "@/components/tag-categories/forms/edit-tag-category-form";
import SelectTenant from "@/components/tag-categories/select-tenant";
import { TagCategory } from "@/server/models/tag-category";
import { getAllTagCategories } from "@/server/queries/tag-categories/queries/get-all-tag-categories";
import { getAllTenantsQuery } from "@/server/queries/tenants";

interface SearchParams {
  searchParams?: {
    tenantCode?: string;
  };
}

export default async function TagCategoriesPage({
  searchParams,
}: SearchParams) {
  const tenants = await getAllTenantsQuery();

  const tenantCode = searchParams?.tenantCode
    ? parseInt(searchParams.tenantCode)
    : null;

  let tagCategories: TagCategory[] | null = null;

  if (tenantCode) {
    tagCategories = await getAllTagCategories(tenantCode);
  }

  function renderTagCategories() {
    if (!tenantCode) {
      return <div>Please select a tenant</div>;
    }

    if (tagCategories!.length === 0) {
      return <div>This tenant doesn't have tag categories</div>;
    }

    return tagCategories!.map((tagCategory) => (
      <EntityCard
        key={tagCategory.code}
        actions={
          <>
            <Dialog
              formAction={editTagCategory}
              formTitle="Edit Tag Category"
              buttonType="edit"
              inputForm={EditTagCategoryForm}
              model={{
                tenantCode,
                code: tagCategory.code,
              }}
            />
            <Dialog
              formAction={deleteTagCategory}
              formTitle="Delete Tag Category"
              buttonType="delete"
              inputForm={DeleteTagCategoryForm}
              model={{
                tenantCode,
                code: tagCategory.code,
              }}
            />
          </>
        }
        entity={tagCategory}
      />
    ));
  }

  return (
    <div>
      <SelectTenant tenants={tenants} />

      {tenantCode && (
        <Dialog
          formAction={createTagCategory}
          formTitle="Create Tag Category"
          buttonType="create"
          inputForm={CreateTagCategoryForm}
          model={{
            tenantCode,
          }}
        />
      )}

      <div className="grid grid-cols-4 gap-4">{renderTagCategories()}</div>
    </div>
  );
}
