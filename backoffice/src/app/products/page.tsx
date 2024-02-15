import SelectTenant from "@/components/tag-categories/select-tenant";
import prisma from "@/database";
import Image from "next/image";
import { MdClose, MdDelete, MdEdit } from "react-icons/md";

interface SearchParams {
  searchParams?: {
    tenantId?: string;
  };
}

export default async function ProductsPage({ searchParams }: SearchParams) {
  const tenantId = searchParams?.tenantId
    ? parseInt(searchParams.tenantId)
    : null;

  const tenants = await prisma.tenant.findMany();

  if (tenantId) {
  }

  return (
    <div>
      <SelectTenant tenants={tenants} />

      <div className="">
        <div className="card w-1/6 bg-base-100 shadow-xl">
          <figure>
            <Image
              src="https://daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.jpg"
              width={300}
              height={300}
              alt="Shoes"
            />
          </figure>
          <div className="card-body">
            <h2 className="card-title">Frango Grelhado</h2>
            <p>O segredo da casa com compras do chefe</p>
            <div className="collapse bg-base-200">
              <input type="checkbox" />
              <div className="collapse-title">Tags</div>
              <div className="collapse-content">
                <p className="flex items-center gap-1 justify-between">
                  Picante{" "}
                  <MdClose className="text-red-500 hover:text-red-400 hover:cursor-grabbing" />
                </p>
                <p className="flex items-center gap-1 justify-between">
                  Picante{" "}
                  <MdClose className="text-red-500 hover:text-red-400 hover:cursor-grabbing" />
                </p>
              </div>
            </div>
            <div className="flex gap-2">
              <MdEdit />
              <MdDelete />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
