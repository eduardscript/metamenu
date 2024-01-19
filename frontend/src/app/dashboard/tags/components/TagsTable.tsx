"use client";

import clsx from "clsx";
import { useState } from "react";

type TagsTableProps = {
  codes: string[];
  onTagSelected: (tag: string | null) => void;
};

const TagsTable: React.FC<TagsTableProps> = (props) => {
  const [selectedTag, setSelectedTag] = useState<string | null>(null);

  const handleTagClicked = (e: React.MouseEvent<HTMLTableRowElement>) => {
    if (selectedTag === e.currentTarget.firstChild!.textContent!) {
      setSelectedTag(null!);
      props.onTagSelected(null!);

      return;
    }

    setSelectedTag(e.currentTarget.firstChild!.textContent!);
    props.onTagSelected(e.currentTarget.firstChild!.textContent!);
  };

  return (
    <table className="w-full">
      <thead className="text-white">
        <tr className="bg-gray-800">
          <th className="border py-3 px-4">Name</th>
          <th className="border py-2 px-4">Number of products with tag</th>
          <th className="border py-2 px-4">Audit data</th>
        </tr>
      </thead>
      <tbody>
        {props.codes.map((code) => (
          <tr key={code} className={clsx("text-black", {
            "bg-white": selectedTag !== code,
            "bg-green-200": selectedTag === code
          })} onClick={handleTagClicked}>
            <td className="border p-4">{code}</td>
            <td className="border p-4">(to be implemented)</td>
            <td className="border p-4">(to be implemented)</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}


export default TagsTable;