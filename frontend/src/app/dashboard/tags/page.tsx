"use client";

import TagsTable from "./components/TagsTable";
import { useEffect, useState } from 'react';
import clsx from 'clsx';
import TenantDropdown from "../tagcategories/ui/TenantDropdown";
import { createTag, deleteTag, getAllTagCategories, getAllTagsByTagCategoryCode, getTagCategories, getTenants } from "@/data/api";
import Modal from "@/app/ui/Modal";
import CreateTagModalForm from "./components/CreateTagModalForm";

type Tenant = {
  code: number;
  name: string;
};

const Tags: React.FC = () => {
  const [tenantsData, setTenantsData] = useState<Tenant[]>([]);
  const [selectedTag, setSelectedTag] = useState<string | null>(null);
  const fetchData = async () => {
    const tenants = await getTenants();
    setTenantsData(tenants);
  };

  useEffect(() => {
    fetchData()
  }, []);

  const [selectedTagCategory, setSelectedTagCategory] = useState<string>(null!);
  const [selectedTenant, setSelectedTenant] = useState<Tenant | null>(null);

  const handleTagCategoryClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (selectedTagCategory === e.currentTarget.textContent!) {
      setSelectedTagCategory(null!);
      return;
    }

    setSelectedTagCategory(e.currentTarget.textContent!);

    getAllTagsByTagCategoryCode(selectedTenant!.code, e.currentTarget.textContent!).then((tags) => {
      setTags(tags);
    });
  }

  const [tagCategories, setTagCategories] = useState<{ code: string }[]>(null!);

  const [tags, setTags] = useState<{ code: string }[]>(null!);
  const handleSelectedTenantChange = (selectedTenant: Tenant | null) => {
    setSelectedTenant(selectedTenant);
    setSelectedTagCategory(null!);
    setSelectedTag(null!);

    getAllTagCategories(selectedTenant!.code).then((tagCategories) => {
      setTagCategories(tagCategories);
    });
  }

  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  const handleCreate = async (tag: { code: string }) => {
    await createTag(selectedTenant!.code, selectedTagCategory, tag.code);

    getAllTagsByTagCategoryCode(selectedTenant!.code, selectedTagCategory).then((tags) => {
      setTags(tags);
    });

    setIsModalOpen(false);
  }

  const handleDelete = async () => {
    await deleteTag(selectedTenant!.code, selectedTagCategory, selectedTag!);

    getAllTagsByTagCategoryCode(selectedTenant!.code, selectedTagCategory).then((tags) => {
      setTags(tags);
    });
  }

  const renderInfoMessage = () => {
    if (selectedTenant === null) {
      return <p className="text-center">Please select a <span className="font-semibold">Tenant</span> to view tags.</p>;
    }

    if (selectedTagCategory === null) {
      return <p className="text-center">Please select a <span className="font-semibold">Tag Category</span> to view tags.</p>;
    }

    if (tags?.length === 0) {
      return <p className="text-center">No tags found for the <span className="font-semibold">{selectedTagCategory}</span>. Please <span className="py-1 px-1 text-white rounded bg-green-500">Create</span> one!</p>;
    }
  }

  return (
    <>
      <h1 className="text-2xl font-bold mb-4">Tags</h1>
      <div className="flex gap-2">
        <div className="flex flex-col">
          {tagCategories?.length > 0 && tagCategories.map((tagCategory) => (
            <button className={clsx("bg-black text-white px-4 py-2 rounded mb-1 hover:bg-green-500", {
              "bg-green-500": tagCategory.code === selectedTagCategory
            })} onClick={handleTagCategoryClicked}>{tagCategory.code}</button>
          ))}
        </div>
        <div className="grow">
          <div className="flex gap-2">
            <TenantDropdown tenants={tenantsData} onChange={handleSelectedTenantChange} className="grow" />
            {isModalOpen && (
              <Modal onClose={() => setIsModalOpen(false)}>
                <CreateTagModalForm onSubmit={handleCreate} onCancel={() => setIsModalOpen(false)} />
              </Modal>
            )}
            <button className={clsx("text-white px-4 py-2 rounded mb-2", {
              "bg-green-200": selectedTenant === null || selectedTagCategory === null,
              "bg-green-500": selectedTenant !== null && selectedTagCategory !== null,
            })} disabled={selectedTenant === null} onClick={() => setIsModalOpen(true)}>Create</button>
            <button className={clsx("text-white px-4 py-2 rounded mb-2", {
              "bg-gray-200": selectedTag === null,
              "bg-gray-500": selectedTag !== null,
            })} disabled={selectedTagCategory === null}>Update</button>
            <button className={clsx("text-white px-4 py-2 rounded mb-2", {
              "bg-gray-100": selectedTag === null,
              "bg-gray-400": selectedTag !== null,
            })} disabled={selectedTagCategory === null} onClick={handleDelete}>Delete</button>
          </div>
          {
            (selectedTagCategory && tags?.length > 0)
             ? <TagsTable codes={tags.map((t) => t.code)} onTagSelected={(tag) => setSelectedTag(tag)} />
             : renderInfoMessage()
          }
        </div>
      </div>
    </>
  );
};

export default Tags;