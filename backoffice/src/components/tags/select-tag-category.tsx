"use client";

import Select from "react-select";
import { usePathname, useSearchParams, useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import { TagCategory } from "@/server/models/tag-category";

export default function SelectTagCategory({
  tagCategories,
  isQuery,
  onChange,
}: {
  tagCategories: TagCategory[];
  isQuery: boolean;
  onChange?: (value: string) => void;
}) {
  const [option, setOption] = useState<{ value: string; label: string }>({
    value: null!,
    label: "",
  });

  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const tagCategoryCodeQuery = searchParams.get("tagCategoryCode");

  useEffect(() => {
    if (tagCategoryCodeQuery) {
      const selectedOption = options.find(
        (o) => o.value === tagCategoryCodeQuery
      );
      if (selectedOption) {
        setOption(selectedOption);
      }
    }
  }, [tagCategoryCodeQuery]);

  const options = tagCategories.map((c) => ({
    value: c.code,
    label: `${c.code}`,
  }));

  function handleSearch(value: string) {
    const params = new URLSearchParams(searchParams);

    value !== "" && value.length > 0
      ? params.set("tagCategoryCode", value.toString())
      : params.delete("tagCategoryCode");

    replace(`${pathname}?${params.toString()}`);
  }

  return (
    <Select
      name="tagCategoryCode"
      className="flex-grow"
      options={options}
      onChange={(p) => {
        isQuery
          ? handleSearch(p?.value || "")
          : setOption(p || { value: null!, label: "teste" });

        if (onChange) {
          onChange(p!.value);
        }
      }}
      placeholder="Select a tag category"
      value={option}
    />
  );
}
