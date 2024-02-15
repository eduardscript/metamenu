import createTag from "@/actions/tags/create-tag";
import deleteTag from "@/actions/tags/delete-tag";
import editTag from "@/actions/tags/edit-tag";
import Dialog from "@/components/common/dialog/dialog";
import EntityCard from "@/components/common/entity-card";
import SelectTenant from "@/components/tag-categories/select-tenant";
import CreateTagForm from "@/components/tags/forms/create-tag-form";
import DeleteTagForm from "@/components/tags/forms/delete-tag-form";
import EditTagForm from "@/components/tags/forms/edit-tag-form";
import SelectTagCategory from "@/components/tags/select-tag-category";
import { TagCategory } from "@/server/models/tag-category";
import { getAllTagCategories } from "@/server/queries/tag-categories/queries/get-all-tag-categories";
import { getAllTagsByTagCategory } from "@/server/queries/tags/queries/get-tags-by-tenant-code";
import { getAllTenantsQuery } from "@/server/queries/tenants";

interface SearchParams {
  searchParams?: {
    tenantCode?: string;
    tagCategoryCode?: string;
  };
}

export default async function TagsPage({ searchParams }: SearchParams) {
  const tenants = await getAllTenantsQuery();

  const tenantCode = searchParams?.tenantCode
    ? parseInt(searchParams.tenantCode)
    : null;

  const tagCategoryCode = searchParams?.tagCategoryCode;

  let tagCategories: TagCategory[] | null = null;

  if (tenantCode) {
    tagCategories = await getAllTagCategories(tenantCode);
  }

  async function renderTagCategories() {
    if (!tenantCode) {
      return <div>Please select a tenant</div>;
    }

    if (tagCategories!.length === 0) {
      return <div>This tenant doesn't have tag categories</div>;
    }

    if (tagCategoryCode && tagCategories) {
      const tags = await getAllTagsByTagCategory(tenantCode, tagCategoryCode);

      return tags!.map((tag) => (
        <EntityCard
          key={tag.code}
          actions={
            <>
              <Dialog
                formTitle="Edit Tag"
                buttonType="edit"
                formAction={editTag}
                inputForm={EditTagForm}
                model={{
                  tag,
                  tagCategories,
                }}
              />
              <Dialog
                formTitle="Delete Tag"
                buttonType="delete"
                formAction={deleteTag}
                inputForm={DeleteTagForm}
                model={{ ...tag, tenantCode, tagCategoryCode }}
              />
            </>
          }
          entity={tag}
        />
      ));
    }
  }

  return (
    <>
      <SelectTenant tenants={tenants} />

      {tenantCode && (
        <SelectTagCategory isQuery tagCategories={tagCategories!} />
      )}

      {tenantCode && tagCategoryCode && (
        <Dialog
          formTitle="Create Tag"
          buttonType="create"
          formAction={createTag}
          inputForm={CreateTagForm}
          model={{
            tenantCode: tenantCode,
            tagCategoryCode: tagCategoryCode,
          }}
        />
      )}

      <div className="grid grid-cols-4 gap-4">{renderTagCategories()}</div>
    </>
  );
}
