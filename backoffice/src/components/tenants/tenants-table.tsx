import EntityCard from "../common/entity-card";
import editTenant from "@/actions/tenants/edit-tenant";
import Dialog from "../common/dialog/dialog";
import EditTenantForm from "./forms/edit-tenant-form";
import deleteTenant from "@/actions/tenants/delete-tenant";
import DeleteTenantForm from "./forms/delete-tenant-form";
import { getAllTenantsQuery } from "@/server/queries/tenants";

export default async function TenantsTable({
  tenantQuery,
}: {
  tenantQuery: string;
}) {
  // TODO:EC: Implement the ability to filter tenants by name

  const filteredCateogires = await getAllTenantsQuery();

  return (
    <div className="grid grid-cols-4 gap-4 mt-4">
      {filteredCateogires.map((tenant) => (
        <EntityCard
          key={tenant.code}
          actions={
            <>
              <Dialog
                formAction={editTenant}
                formTitle="Edit Tenant"
                buttonType="edit"
                inputForm={EditTenantForm}
                model={tenant}
              />
              <Dialog
                formAction={deleteTenant}
                formTitle="Delete Tenant"
                buttonType="delete"
                inputForm={DeleteTenantForm}
                model={tenant}
              />
            </>
          }
          entity={tenant}
          additionalActions={
            <div
              className={`badge uppercase text-white font-bold p-4 ${
                tenant.isEnabled ? "bg-green-500" : "bg-red-500"
              }`}
            >
              {tenant.isEnabled ? "Enabled" : "Inactive"}
            </div>
          }
        />
      ))}
    </div>
  );
}
