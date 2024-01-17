"use client";

import DashboardLayout from "@/app/ui/dashboard_layout";
import TenantDropdown from "./ui/TenantDropdown";
import { useEffect, useState } from "react";
import TagCategoryCard from "./ui/TagCategoryCard";
import Modal from "@/app/ui/Modal";
import NewTagCategoryForm from "./ui/NewTagCategoryForm";
import { createTagCategory, getTagCategories, getTenants } from "@/data/api";

// const tenantsData = [
//   { code: 1000, name: 'Tasca do Spezas' },
//   { code: 2000, name: 'Restaurante Malmequer' },
// ];

// const tagCategoriesData = [
//   { code: "Ingredients" },
//   { code: "MenuKind" },
// ];

const TagCategories: React.FC = () => {
  const [tenantsData, setTenantsData] = useState<any[]>([]);
  const [tagCategoriesData, setTagCategoriesData] = useState<any[]>([]);

  const fetchData = async () => {
    const { data } = await getTenants();
    setTenantsData(data.allTenants);
  };

  useEffect(() => {
    fetchData()
  }, []);

  const [selectedTenant, setSelectedTenant] = useState<{ code: number; name: string } | null>(null);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  const handleTenantChange = async (selectedTenant: { code: number; name: string } | null) => {
    const { allTagCategories } = await getTagCategories(selectedTenant!.code);

    setSelectedTenant(selectedTenant);
    setTagCategoriesData(allTagCategories);
  };

  const handleDelete = async () => {
  };

  const handleEdit = async () => {
  };

  const handleCreate = async (tagCategory: { code: string }) => {
    console.log(`Creating tag category with code: ${tagCategory.code}`);

    await createTagCategory(selectedTenant!.code, tagCategory.code);

    const { allTagCategories } = await getTagCategories(selectedTenant!.code);

    setTagCategoriesData(allTagCategories);

    setIsModalOpen(false);
  }

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-bold mb-4">Tag Categories</h2>
      <TenantDropdown tenants={tenantsData} onChange={handleTenantChange} />
      {isModalOpen && (
          <Modal onClose={() => setIsModalOpen(false)}>
            <NewTagCategoryForm onSubmit={handleCreate} onCancel={() => setIsModalOpen(false)} />
          </Modal>
        )}
      {selectedTenant &&
        <>
          <button className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded my-4" onClick={() => setIsModalOpen(!isModalOpen)}>
            Create Tag Category
          </button>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mt-4">
            {tagCategoriesData.length > 0 ? tagCategoriesData.map((tagCategory) => (
              <TagCategoryCard key={tagCategory.code} code={tagCategory.code} onDelete={handleDelete} onEdit={handleEdit} />
            )) : <p>This tenant doesn't have tag categories.</p>}
          </div>
        </>}
    </DashboardLayout>
  );
};

export default TagCategories;