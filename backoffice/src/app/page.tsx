import TenantsTable from "@/components/tenants/tenants-table";
import SearchInput from "@/components/search-input";
import { Suspense } from "react";
import Dialog from "@/components/common/dialog/dialog";
import createTenant from "@/actions/tenants/create-tenant";
import CreateTenantForm from "@/components/tenants/forms/create-tenant-form";

export default function Page({
  searchParams,
}: {
  searchParams?: {
    tenant?: string;
  };
}) {
  const tenantQuery = searchParams?.tenant || "";

  return (
    <div className="flex-col justify-items-center">
      <div className="flex gap-2 items-center">
        <SearchInput />

        <Dialog
          formAction={createTenant}
          formTitle="Create a new Tenant"
          buttonType="create"
          inputForm={CreateTenantForm}
        />
      </div>

      <Suspense
        key={tenantQuery}
        fallback={
          <span className="flex items-center justify-center loading loading-spinner loading-lg"></span>
        }
      >
        <TenantsTable tenantQuery={tenantQuery} />
      </Suspense>
    </div>
  );
}
