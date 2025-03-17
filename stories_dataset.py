import pandas as pd

'''
file = pd.read_csv("final_ff_1.csv", sep=";")
file["age"] = pd.to_numeric(file["age"], errors="coerce")
file["share_tap_flg"] = pd.to_numeric(file["share_tap_flg"], errors="coerce").fillna(0)

min_age = int(file["age"].min())
max_age = int(file["age"].max())

start = min_age - (min_age % 5)
end = max_age + (5 - max_age % 5)
bins = list(range(start, end + 1, 5))

labels = [f"({bins[i]}-{bins[i+1]}]" for i in range(len(bins) - 1)]
file["age_group"] = pd.cut(file["age"], bins = bins, right = True, labels = labels)
grouped = file.groupby("age_group", observed=False)["share_tap_flg"].sum()

max_group = grouped.idxmax()
max_value = grouped.max()

print(max_group)
print(max_value)
'''
'''
file = pd.read_csv("final_ff_1.csv", sep=";")
file["like_tap_flg"] = pd.to_numeric(file["like_tap_flg"], errors="coerce").fillna(0)
file["prosmotr"] = pd.to_numeric(file["prosmotr"], errors="coerce")

file["location"] = file["geo_country"].apply(lambda x: "inside country" if x == "RU" else "outside country")
grouped = file.groupby("location").agg({"like_tap_flg": "sum", "prosmotr": "sum"})
grouped["like_rate"] = grouped["like_tap_flg"] / grouped["prosmotr"]

max_location = grouped["like_rate"].idxmax()
max_rate = grouped.loc[max_location, "like_rate"] * 100
max_rate = round(max_rate, 3)

print(max_location)
print(max_rate)
'''

file = pd.read_csv("final_ff_1.csv", sep=";")
file["prosmotr"] = pd.to_numeric(file["prosmotr"], errors="coerce").fillna(0)
file["favorite_tap_flg"] = pd.to_numeric(file["favorite_tap_flg"], errors="coerce").fillna(0)

grouped = file.groupby("story_id").agg(
    total_prosmotr = ("prosmotr", "sum"),
    total_favourite = ("favorite_tap_flg", "sum"),
    name = ("name", "first"))

filtered = grouped[grouped["total_prosmotr"] > 100].copy()
filtered["conversion"] = filtered["total_favourite"] / filtered["total_prosmotr"]
max_story = filtered.loc[filtered["conversion"].idxmax()]

print(max_story["name"])
print(round(max_story["conversion"] * 100, 3))

