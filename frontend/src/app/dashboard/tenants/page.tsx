"use client";

import DashboardLayout from "@/app/ui/dashboard_layout";
import TenantsTable from "./ui/TenantsTable";
import { GET_TENANTS } from "@/data/queries";
import { getTenants, createTenant, deleteTenant, toggleTenantStatus } from "@/data/api";
import { useEffect, useState } from "react";

const Tenants: React.FC = () => {
  const [tenantsData, setTenantsData] = useState<any[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);

  const fetchData = async (page: number) => {
    try {
      const { data } = await getTenants();
      setTenantsData(data.allTenants);
    } catch (error) {
    }
  };

  useEffect(() => {
    fetchData(currentPage);
  }, []);

  const handleDelete = async (code: number) => {
    console.log(`Deleting tenant with code: ${code}`);

    await deleteTenant(code);

    fetchData(currentPage);
  };

  const handleDeactivate = async (code: number, isEnabled: boolean) => {
    console.log(`Deactivating tenant with code: ${code}`);

    await toggleTenantStatus(code, isEnabled);

    await fetchData(currentPage);
  };

  const handleCreate = async (tenant: { name: string }) => {
    console.log(`Creating tenant with name: ${tenant.name}`);

    const { data } = await createTenant(tenant.name);

    console.log(`Tenant created with code: ${data.createTenant.code}`);

    fetchData(currentPage);
  }

  useEffect(() => {
    fetchData(currentPage);
  }, [currentPage]);

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-bold mb-4">Tenants</h2>
      <TenantsTable
        tenants={tenantsData}
        onDelete={handleDelete}
        onDeactivate={handleDeactivate}
        onCreate={handleCreate} />
    </DashboardLayout>
  );
};

export default Tenants;