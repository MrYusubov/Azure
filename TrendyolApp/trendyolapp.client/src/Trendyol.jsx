import React, { useState } from "react";
import axios from "axios";

const Trendyol = () => {
  const [discountCode, setDiscountCode] = useState("");
  const [used, setUsed] = useState(false);
  const [applied, setApplied] = useState(false);

  const applyDiscount = async () => {
    try {
      const response = await axios.get("https://localhost:7160/api/Discount/get-discount");
      setDiscountCode(response.data.code);
      setUsed(true);
    } catch {
      alert("Discount doesn't exist");
    }
  };

  const cancelDiscount = () => {
    setDiscountCode("");
    setUsed(false);
    setApplied(false);
  };

  return (
    <div>
      <h2>TrendYol App</h2>
      {!used ? (
        <button onClick={applyDiscount}>Apply Discount</button>
      ) : (
        <>
          <p>Discount Code: {discountCode}</p>
          <button disabled>Discount Code succesifully added</button>
          <button onClick={cancelDiscount}>Remove Code</button>
        </>
      )}
    </div>
  );
};

export default Trendyol;
