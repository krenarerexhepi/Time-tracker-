using System;

namespace TimeTrackerMobile{

 public class StringArrayAdapter : BaseAdapter {

		List<Model> listOfData;
		LayoutInflater layoutInflater;

		public StringArrayAdapter(List<Model> data, Context c) 
		{
    
			listOfData = listOfData == null ? new List<Model>(data) : data;
         layoutInflater = (LayoutInflater) c.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
   		 }

    public StringArrayAdapter(String[] data, Context c) {
        if (listOfData == null)
            listOfData = new ArrayList<Model>();

        for (String aData : data) {
            listOfData.add(new Model(aData));
        }

        layoutInflater = (LayoutInflater) c.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    public int getCount() {
        return listOfData.size();
    }

    public Object getItem(int i) {
        return listOfData.get(i);
    }

    public Object getItemString(int i) {
        return listOfData.get(i).getName();
    }

    public long getItemId(int i) {
        return 0;
    }

    public View getView(int position, View view, ViewGroup viewGroup) {

        if (view == null)
            view = layoutInflater.inflate(android.R.layout.simple_list_item_1, viewGroup, false);

        Model currentModel = listOfData.get(position);
        String name = currentModel.getName();


        ((TextView) view.findViewById(android.R.id.text1)).setText(name);


        return view;

    }

    public void clear() {
        listOfData = new ArrayList<Model>();
        notifyDataSetChanged();
    }

    public void changeData(ArrayList<Model> data) {
        listOfData = data;
        if (listOfData == null)
            listOfData = new ArrayList<Model>();
        notifyDataSetChanged();
    }

  }
}
